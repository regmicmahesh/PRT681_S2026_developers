import { fileURLToPath } from 'node:url';
import { dirname, join } from 'node:path';

import { createDatabase } from './data/database.js';
import { createCareerTrackServer } from './http/app.js';

const currentDirectory = dirname(fileURLToPath(import.meta.url));
const projectDirectory = join(currentDirectory, '..');
const port = Number.parseInt(process.env.PORT ?? '3000', 10);
const database = createDatabase(join(projectDirectory, 'data', 'careertrack.db'));
const server = createCareerTrackServer({
  database,
  publicDirectory: join(projectDirectory, 'public'),
});

server.listen(port, '127.0.0.1', () => {
  console.log(`CareerTrack NT is running at http://127.0.0.1:${port}`);
});

function shutdown() {
  server.close(() => {
    database.close();
    process.exit(0);
  });
}

process.on('SIGINT', shutdown);
process.on('SIGTERM', shutdown);
