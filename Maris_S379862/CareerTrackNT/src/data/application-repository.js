function toApplication(row) {
  if (!row) {
    return null;
  }

  return {
    id: Number(row.id),
    company: row.company,
    role: row.role,
    status: row.status,
    applicationDate: row.application_date,
    followUpDate: row.follow_up_date,
    jobUrl: row.job_url,
    notes: row.notes,
    createdAt: row.created_at,
    updatedAt: row.updated_at,
  };
}

export function createApplicationRepository(database) {
  const selectById = database.prepare('SELECT * FROM applications WHERE id = ?');
  const insert = database.prepare(`
    INSERT INTO applications (
      company, role, status, application_date, follow_up_date,
      job_url, notes, created_at, updated_at
    ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)
  `);
  const update = database.prepare(`
    UPDATE applications
    SET company = ?, role = ?, status = ?, application_date = ?,
        follow_up_date = ?, job_url = ?, notes = ?, updated_at = ?
    WHERE id = ?
  `);
  const remove = database.prepare('DELETE FROM applications WHERE id = ?');
  const summary = database.prepare(`
    SELECT status, COUNT(*) AS count
    FROM applications
    GROUP BY status
  `);

  return {
    create(application, timestamp) {
      const result = insert.run(
        application.company,
        application.role,
        application.status,
        application.applicationDate,
        application.followUpDate,
        application.jobUrl,
        application.notes,
        timestamp,
        timestamp,
      );
      return toApplication(selectById.get(Number(result.lastInsertRowid)));
    },

    getById(id) {
      return toApplication(selectById.get(id));
    },

    list({ status, search, page, pageSize }) {
      const clauses = [];
      const parameters = [];

      if (status) {
        clauses.push('status = ?');
        parameters.push(status);
      }

      if (search) {
        clauses.push('(company LIKE ? COLLATE NOCASE OR role LIKE ? COLLATE NOCASE)');
        const term = `%${search}%`;
        parameters.push(term, term);
      }

      const where = clauses.length > 0 ? `WHERE ${clauses.join(' AND ')}` : '';
      const totalRow = database
        .prepare(`SELECT COUNT(*) AS count FROM applications ${where}`)
        .get(...parameters);
      const offset = (page - 1) * pageSize;
      const rows = database
        .prepare(`
          SELECT * FROM applications
          ${where}
          ORDER BY COALESCE(follow_up_date, '9999-12-31') ASC, id DESC
          LIMIT ? OFFSET ?
        `)
        .all(...parameters, pageSize, offset);

      return {
        items: rows.map(toApplication),
        total: Number(totalRow.count),
      };
    },

    update(id, application, timestamp) {
      const result = update.run(
        application.company,
        application.role,
        application.status,
        application.applicationDate,
        application.followUpDate,
        application.jobUrl,
        application.notes,
        timestamp,
        id,
      );
      return Number(result.changes) === 0 ? null : toApplication(selectById.get(id));
    },

    remove(id) {
      return Number(remove.run(id).changes) > 0;
    },

    summary() {
      return summary.all().map((row) => ({
        status: row.status,
        count: Number(row.count),
      }));
    },
  };
}
