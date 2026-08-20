import { useQuery } from '@tanstack/react-query';
import { jobsApi } from '../../infrastructure/api/jobsApi';
import { jobKeys } from './queryKeys';

export function useJobs() {
  return useQuery({
    queryKey: jobKeys.lists(),
    queryFn: jobsApi.getAll,
  });
}
