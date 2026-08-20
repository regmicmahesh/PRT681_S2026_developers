export type EmploymentType = 'FullTime' | 'PartTime' | 'Contract' | 'Internship';

export type PayPeriod = 'Hourly' | 'Daily' | 'Weekly' | 'Monthly' | 'Annually';

export type JobStatus = 'Draft' | 'Published' | 'Closed';

export interface Job {
  id: string;
  title: string;
  description: string;
  employmentType: EmploymentType;
  salaryMin: number;
  salaryMax: number;
  salaryCurrency: string;
  payPeriod: PayPeriod;
  status: JobStatus;
  companyId: string;
  applicationCount: number;
}
