import { useMutation, useQueryClient } from '@tanstack/react-query';
import { jobsApi } from '../../infrastructure/api/jobsApi';
import { jobKeys } from './queryKeys';

export function useCloseJob() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => jobsApi.close(id),
    onSuccess: (_data, id) => {
      queryClient.invalidateQueries({ queryKey: jobKeys.detail(id) });
      queryClient.invalidateQueries({ queryKey: jobKeys.lists() });
    },
  });
}
