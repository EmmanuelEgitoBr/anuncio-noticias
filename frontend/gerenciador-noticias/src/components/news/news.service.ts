import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface News {
  id?: string;
  hat: string;
  title: string;
  text: string;
  author: string;
  publishDate: Date;
  image: string;
  link: string;
  status: number;
}

@Injectable({ providedIn: 'root' })
export class NewsService {
  private apiUrl = 'https://localhost:7118/api/news';

  constructor(private http: HttpClient) {}

  getAll(): Observable<News[]> {
    return this.http.get<News[]>(this.apiUrl);
  }

  getById(id: string): Observable<News> {
    return this.http.get<News>(`${this.apiUrl}/${id}`);
  }

  create(news: News): Observable<News> {
    return this.http.post<News>(`${this.apiUrl}/create`, news);
  }

  update(id: string, news: News): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/update/${id}`, news);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete/${id}`);
  }
}
