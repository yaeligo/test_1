import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RepositoryItem } from '../models/repository.model';

@Injectable({
  providedIn: 'root'
})
export class GithubService {
  private readonly apiUrl = 'http://localhost:5000/api/repos';

  constructor(private http: HttpClient) {}

  searchRepositories(query: string): Observable<any> {
  return this.http.get<any>(`${this.apiUrl}/search?query=${encodeURIComponent(query)}`);
}
}