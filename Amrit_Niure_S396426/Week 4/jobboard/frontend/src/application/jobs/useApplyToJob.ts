import { useMutation, useQueryClient } from '@tanstack/react-query';
import { jobsApi, type ApplyToJobRequest } from '../../infrastructure/api/jobsApi';
import { jobKeys } from './queryKeys';

export function useApplyToJob(jobId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: ApplyToJobRequest) => jobsApi.apply(jobId, request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: jobKeys.detail(jobId) });
      queryClient.invalidateQueries({ queryKey: jobKeys.lists() });
    },
  });
}
