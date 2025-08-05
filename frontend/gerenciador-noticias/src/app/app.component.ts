import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NewsComponent } from "../components/news/news.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NewsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'gerenciador-noticias';
}
