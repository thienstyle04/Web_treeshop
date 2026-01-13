import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CategoryService } from '../../core/services/category.service';
import { Category } from '../../core/models';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.css']
})
export class CategoriesComponent implements OnInit {
  private categoryService = inject(CategoryService);

  categories = signal<Category[]>([]);
  loading = signal(true);

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getAllCategories().subscribe({
      next: (categories) => {
        // Filter out "Chậu Cây Cảnh" category
        const filteredCategories = categories.filter(c => c.name !== 'Chậu Cây Cảnh');
        this.categories.set(filteredCategories);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      }
    });
  }

  getCategoryIcon(name: string): string {
    const icons: { [key: string]: string } = {
      'cây cảnh': '🌿',
      'cây phong thủy': '🎋',
      'cây văn phòng': '🪴',
      'cây nội thất': '🌱',
      'hoa': '🌸',
      'sen đá': '🪨',
      'xương rồng': '🌵',
      'bonsai': '🎍'
    };
    const lowerName = name.toLowerCase();
    for (const [key, icon] of Object.entries(icons)) {
      if (lowerName.includes(key)) return icon;
    }
    return '🌿';
  }
}
