export const jobKeys = {
  all: ['jobs'] as const,
  lists: () => [...jobKeys.all, 'list'] as const,
  detail: (id: string) => [...jobKeys.all, 'detail', id] as const,
};
