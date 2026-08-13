import { useMutation, useQueryClient } from '@tanstack/react-query';
import { jobsApi, type CreateJobRequest } from '../../infrastructure/api/jobsApi';
import { jobKeys } from './queryKeys';

export function useCreateJob() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (request: CreateJobRequest) => jobsApi.create(request),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: jobKeys.lists() });
    },
  });
}
