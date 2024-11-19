import { Component, inject } from '@angular/core';
import { MatDivider } from '@angular/material/divider';
import { MatListOption, MatSelectionList } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { ShopService } from '../../../core/services/shop.service';

@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [MatDivider,MatSelectionList,MatListOption,MatButton,FormsModule],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss'
})
export class FiltersDialogComponent {
  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  data = inject(MAT_DIALOG_DATA);

  selectedBrands: string[] = this.data.selectedBrands;
  selectedTypes: string[] = this.data.selectedTypes;

  //public shopService = inject(ShopService); // There is no private here because shopService is used in the HTML
  // OR  
  constructor(public shopService: ShopService) { // public here because shopService is used in the HTML
  }

  applyFilters() {
    this.dialogRef.close({
      selectedBrands: this.selectedBrands,
      selectedTypes: this.selectedTypes
    })
  }
}
