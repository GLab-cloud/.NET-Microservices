import { Genre } from '../models/Genre';

class GenresClient {
  private baseUrl = '/api';

  async getGenresAsync(): Promise<Genre[]> {
    const response = await this.fetchWithHandling(`${this.baseUrl}/genres`);
    
    if (!response.ok) {
      const errorMessages = await this.handleFetchError(response);
      throw new Error(errorMessages.join('\n'));
    }

    return response.json() as Promise<Genre[]>;
  }

  private async fetchWithHandling(url: string, options?: RequestInit): Promise<Response> {
    try {
      const response = await fetch(url, options);
      return response;
    } catch (error) {
      if (error instanceof TypeError) {
        throw new Error('We are currently experiencing issues loading the data. Please try again later.');
      }
      throw error;
    }
  }

  private async handleFetchError(response: Response): Promise<string[]> {
    let errorMessages: string[] = ['Unknown error'];
    try {
      const errorData = await response.json();
      if (errorData.title) {
        errorMessages = [errorData.title];
        if (errorData.errors && Array.isArray(errorData.errors)) {
          errorMessages = errorMessages.concat(errorData.errors);
        }
      } else if (errorData.errors && Array.isArray(errorData.errors)) {
        errorMessages = errorData.errors;
      } else if (errorData.detail) {
        errorMessages = [errorData.detail];
      }
    } catch (e) {
      console.error('Error parsing error response:', e);
    }
    return errorMessages;
  }  
}

export default GenresClient;