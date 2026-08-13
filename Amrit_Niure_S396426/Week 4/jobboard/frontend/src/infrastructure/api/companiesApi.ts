import { httpClient } from './httpClient';

export interface CreateCompanyRequest {
  name: string;
  contactEmail: string;
}

export const companiesApi = {
  create: (request: CreateCompanyRequest) => httpClient.post<string>('/companies', request),
};
