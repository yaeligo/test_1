export interface RepositoryItem {
  id: number;
  name: string;
  description?: string;
  html_url?: string;
  htmlUrl?: string;
  stargazers_count?: number;
  stargazersCount?: number;
  owner?: {
    login: string;
    avatar_url: string;
    avatarUrl?: string;
  };
}