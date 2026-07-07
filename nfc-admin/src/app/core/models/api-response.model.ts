/** Format standardisé de toutes les réponses de l'API */
export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T | null;
  errors: string[];
}
