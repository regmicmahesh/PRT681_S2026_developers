import { useMutation } from '@tanstack/react-query';
import { companiesApi, type CreateCompanyRequest } from '../../infrastructure/api/companiesApi';

export function useCreateCompany() {
  return useMutation({
    mutationFn: (request: CreateCompanyRequest) => companiesApi.create(request),
  });
}
