import { useQuery } from '@tanstack/react-query';
import { jobsApi } from '../../infrastructure/api/jobsApi';
import { jobKeys } from './queryKeys';

export function useJob(id: string | undefined) {
  return useQuery({
    queryKey: jobKeys.detail(id ?? ''),
    queryFn: () => jobsApi.getById(id!),
    enabled: Boolean(id),
  });
}
