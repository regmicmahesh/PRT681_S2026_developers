export type EmploymentType = 'FullTime' | 'PartTime' | 'Contract' | 'Internship';

export type JobStatus = 'Draft' | 'Published' | 'Closed';

export interface Job {
  id: string;
  title: string;
  description: string;
  employmentType: EmploymentType;
  salaryMin: number;
  salaryMax: number;
  salaryCurrency: string;
  status: JobStatus;
  companyId: string;
  applicationCount: number;
}
