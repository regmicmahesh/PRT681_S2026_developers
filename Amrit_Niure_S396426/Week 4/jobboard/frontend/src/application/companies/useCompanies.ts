import { useQuery } from '@tanstack/react-query';
import { companiesApi } from '../../infrastructure/api/companiesApi';
import { companyKeys } from './queryKeys';

export function useCompanies() {
  return useQuery({
    queryKey: companyKeys.lists(),
    queryFn: companiesApi.getAll,
  });
}
