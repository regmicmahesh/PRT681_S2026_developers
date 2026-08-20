import { httpClient } from './httpClient';
import type { Company } from '../../domain/company';

export interface CreateCompanyRequest {
  name: string;
  contactEmail: string;
}

export const companiesApi = {
  getAll: () => httpClient.get<Company[]>('/companies'),
  create: (request: CreateCompanyRequest) => httpClient.post<string>('/companies', request),
};
