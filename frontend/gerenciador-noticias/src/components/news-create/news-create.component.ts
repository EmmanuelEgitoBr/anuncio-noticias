import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { News, NewsService } from '../news/news.service';
import { HttpClient, HttpEventType } from '@angular/common/http';

@Component({
  selector: 'app-news-create',
  standalone: true, // ⬅️ Importante
  imports: [FormsModule, NgIf],
  templateUrl: './news-create.component.html'
})
export class NewsCreateComponent {
  news: News = {
    hat: '',
    title: '',
    text: '',
    author: '',
    image: '',
    link: '',
    status: 1,
    publishDate: new Date()
  };

  selectedFile?: File;
  imageUrl: string | ArrayBuffer | null = null;

  constructor(private newsService: NewsService, private http: HttpClient, private router: Router) {}

  create(): void {
    this.newsService.create(this.news).subscribe(() => {
      this.router.navigate(['/news']);
    });
  }

  cancel(): void {
    this.router.navigate(['/news']);
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];

      // Exibir preview
      const reader = new FileReader();
      reader.onload = () => {
        this.imageUrl = reader.result;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  uploadImage() {
    if (!this.news.id) {
      alert('Crie a notícia primeiro antes de enviar imagem.');
      return;
    }

    const formData = new FormData();
    formData.append('file', this.selectedFile!);

    this.http.post(`https://localhost:7118/api/news/${this.news.id}/upload-image`, formData, {
      reportProgress: true,
      observe: 'events',
    }).subscribe({
      next: (event) => {
        if (event.type === HttpEventType.Response) {
          alert('Imagem enviada com sucesso!');
        }
      },
      error: () => alert('Erro ao enviar imagem.')
    });
  }
}
