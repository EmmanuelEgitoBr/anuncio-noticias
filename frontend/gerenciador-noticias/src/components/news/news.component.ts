import { Component, OnInit, TemplateRef } from '@angular/core';
import { News, NewsService } from './news.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';

import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { Status } from './status.enum';

@Component({
  selector: 'app-news',
  imports: [CommonModule, FormsModule, NgbModule],
  templateUrl: './news.component.html',
})
export class NewsComponent implements OnInit {
  newsList: News[] = [];
  selectedNews: News = this.initNews();
  Status = Status

  constructor(
    private newsService: NewsService,
    private modalService: NgbModal,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadNews();
  }

  goToCreate(): void {
    this.router.navigate(['/news/create']);
  }

  loadNews() {
    this.newsService.getAll().subscribe(data => this.newsList = data);
  }

  onImageSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0 || !this.selectedNews.id) return;

    const file = input.files[0];
    const formData = new FormData();
    formData.append('file', file);

    fetch(`https://localhost:7118/api/news/${this.selectedNews.id}/upload-image`, {
        method: 'POST',
        body: formData
    })
    .then(async response => {
      if (!response.ok) throw new Error('Falha ao enviar imagem.');
      const result = await response.json(); // supondo que o retorno contenha a nova URL
      this.selectedNews.image = result.url || this.selectedNews.image;
    })
    .catch(err => {
      console.error(err);
      alert('Erro ao fazer upload da imagem.');
    });
  }

  openEditModal(content: TemplateRef<any>, news: News) {
    this.selectedNews = { ...news };
    this.modalService.open(content);
  }

  save() {
    if (this.selectedNews.id) {
      this.newsService.update(this.selectedNews.id, this.selectedNews).subscribe(() => this.loadNews());
    }
  }

  delete(id: string) {
    this.newsService.delete(id).subscribe(() => this.loadNews());
  }

  private initNews(): News {
    return {
      hat: '',
      title: '',
      text: '',
      author: '',
      publishDate: new Date(),
      image: '',
      link: '',
      status: 0
    };
  }
}
