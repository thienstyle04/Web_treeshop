import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'app-about',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './about.component.html',
    styleUrls: ['./about.component.css']
})
export class AboutComponent {
    shopInfo = {
        address: '123 Đường Cây Xanh, Quận Gò Vấp, TP.HCM',
        phone: '090 123 4567',
        email: 'lienhe@treeshop.vn',
        workingHours: '8:00 - 21:00 (Hàng ngày)'
    };
}
