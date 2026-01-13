import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Province {
    name: string;
    code: number;
    division_type: string;
    codename: string;
    phone_code: number;
    districts?: District[];
}

export interface District {
    name: string;
    code: number;
    division_type: string;
    codename: string;
    province_code: number;
    wards?: Ward[];
}

export interface Ward {
    name: string;
    code: number;
    division_type: string;
    codename: string;
    district_code: number;
}

@Injectable({
    providedIn: 'root'
})
export class ProvincesService {
    private http = inject(HttpClient);
    // Using local proxy to bypass CORS - see proxy.conf.json
    private baseUrl = '/api/provinces';

    // Get all provinces
    getProvinces(): Observable<Province[]> {
        return this.http.get<Province[]>(`${this.baseUrl}/p/`);
    }

    // Get province by code with districts (depth=2)
    getProvinceWithDistricts(code: number): Observable<Province> {
        return this.http.get<Province>(`${this.baseUrl}/p/${code}?depth=2`);
    }

    // Get district by code with wards
    getDistrictWithWards(districtCode: number): Observable<District> {
        return this.http.get<District>(`${this.baseUrl}/d/${districtCode}?depth=2`);
    }
}

