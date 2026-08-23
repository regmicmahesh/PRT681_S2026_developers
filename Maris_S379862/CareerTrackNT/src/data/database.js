import { mkdirSync } from 'node:fs';
import { dirname } from 'node:path';
import { DatabaseSync } from 'node:sqlite';

export function createDatabase(path) {
  if (path !== ':memory:') {
    mkdirSync(dirname(path), { recursive: true });
  }

  const database = new DatabaseSync(path);
  database.exec('PRAGMA foreign_keys = ON;');
  database.exec('PRAGMA journal_mode = WAL;');
  database.exec(`
    CREATE TABLE IF NOT EXISTS applications (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      company TEXT NOT NULL CHECK(length(company) BETWEEN 2 AND 100),
      role TEXT NOT NULL CHECK(length(role) BETWEEN 2 AND 120),
      status TEXT NOT NULL CHECK(status IN (
        'Wishlist', 'Applied', 'Interview', 'Offer', 'Rejected', 'Withdrawn'
      )),
      application_date TEXT,
      follow_up_date TEXT,
      job_url TEXT,
      notes TEXT,
      created_at TEXT NOT NULL,
      updated_at TEXT NOT NULL
    );

    CREATE INDEX IF NOT EXISTS idx_applications_status
      ON applications(status);
    CREATE INDEX IF NOT EXISTS idx_applications_follow_up_date
      ON applications(follow_up_date);
  `);

  return database;
}
