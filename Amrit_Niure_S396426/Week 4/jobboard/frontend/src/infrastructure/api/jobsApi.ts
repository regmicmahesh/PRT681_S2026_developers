import { httpClient } from './httpClient';
import type { EmploymentType, Job } from '../../domain/job';

export interface CreateJobRequest {
  title: string;
  description: string;
  employmentType: EmploymentType;
  salaryMin: number;
  salaryMax: number;
  salaryCurrency: string;
  companyId: string;
}

export interface ApplyToJobRequest {
  candidateName: string;
  candidateEmail: string;
  resumeUrl: string;
}

export const jobsApi = {
  getAll: () => httpClient.get<Job[]>('/jobs'),
  getById: (id: string) => httpClient.get<Job>(`/jobs/${id}`),
  create: (request: CreateJobRequest) => httpClient.post<string>('/jobs', request),
  publish: (id: string) => httpClient.post<void>(`/jobs/${id}/publish`),
  close: (id: string) => httpClient.post<void>(`/jobs/${id}/close`),
  apply: (id: string, request: ApplyToJobRequest) =>
    httpClient.post<string>(`/jobs/${id}/applications`, request),
};
