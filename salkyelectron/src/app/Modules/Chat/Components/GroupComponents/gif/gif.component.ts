import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DomSanitizer, SafeResourceUrl, SafeUrl } from '@angular/platform-browser';
import { Gif, GiphyService } from 'src/app/Services/giphy.service';
import { PaginationResult } from 'src/app/Models/PaginationResult';
import { LoaddingService } from 'src/app/Services/loadding.service';

@Component({
  selector: 'app-gif',
  templateUrl: './gif.component.html',
  styleUrls: ['./gif.component.scss'],
})
export class GifComponent implements OnInit {
  constructor(
    private gifService: GiphyService
  ) {
    this.setGifsAsDefault();
  }


  print(any :any){
    console.log(any);
  }

  @Output() sendGifRequest = new EventEmitter<any>();
  public gifs: PaginationResult<Gif> = {} as any;

  setGifsAsDefault() {
    this.gifs = {
      results: [],
      pageIndex: -1,
      lastPage: -1,
      pageSize: -1,
      totalCount: -1,
      totalPages : -1
    };
  }

  @Input() set IsHide(value: boolean) {
    if (!value) {
      this.isSearchResult = false;
      this.gifService.GetTrends().subscribe({
        next: (gifs: any) => {
          this.gifs.results = gifs;
        },
      });
    } else {
      this.setGifsAsDefault();
      this.searchText = '';
    }
    this._isHide = value;
  }
  get IsHide(): boolean {
    return this._isHide;
  }

  _isHide = false;
  public searchText: string = '';
  sleep(ms: number): Promise<void> {
    return new Promise((resolve) => setTimeout(resolve, ms));
  }

  ngOnInit(): void {}

  public keyPress(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }
  private search() {
    this.isSearchResult = true;
    if (this.canSearch()) {
      this.setGifsAsDefault();
      this.gifService.Search(this.searchText, this.gifs).subscribe({
        next: (gifs) => this.setGifs(gifs),
      });
    }
  }
  public goNext() {
    if (this.canGoNext()) {
      this.gifService.Search(this.searchText, this.gifs).subscribe({
        next: (gifs) => this.pushGifs(gifs),
      });
    }
  }
  private setGifs(GifRes: PaginationResult<Gif>) {
    this.gifs.results = [];
    this.gifs = { ...GifRes };
  }
  private pushGifs(GifRes: PaginationResult<Gif>) {
    GifRes.results.forEach((x) => this.gifs.results.push(x));
    this.gifs.pageIndex = GifRes.pageIndex;
    this.gifs.totalCount = GifRes.totalCount;
    this.gifs.pageSize = GifRes.pageSize;
    this.gifs.lastPage = GifRes.lastPage;
  }

  private canGoNext(): boolean {
    return this.canSearch();
  }
  public canSearch(): boolean {
    return this.searchText.length > 2;
  }
  private isSearchResult: boolean = false;
  public get canShowMoreIsEnabled(): boolean {
    return (
      this.gifs.results.length > 0 &&
      this.isSearchResult &&
      this.searchText.length > 0 &&
      this.gifs.pageIndex < this.gifs.lastPage
    );
  }

  isSearching = false;
}
