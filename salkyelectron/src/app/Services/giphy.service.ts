import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable, of, take } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Pagination } from '../Models/Pagination';
import { PaginationResult } from '../Models/PaginationResult';

export interface Gif {
  imageSource: string;
  height: number;
  width: number;
}

@Injectable({
  providedIn: 'root',
})
export class GiphyService {
  constructor(private http: HttpClient) {}

  private MapToGif(httpResult: any): Gif[] {
    return httpResult.data.map((x: any) => {
      return {
        imageSource: x.images.original.url,
        height: x.images.original.height,
        width: x.images.original.width,
      } as Gif;
    });
  }

  public GetTrends(): Observable<Gif[]> {
    let params = new HttpParams().set('api_key', environment.giphyApiKey);
    return this.http
      .get<any>(environment.giphyApiUrl + '/gifs/trending', { params: params })
      .pipe(
        take(1),
        map((res: { data: any[] }) => {
          return this.MapToGif(res);
        })
      );
  }

  private SearchPageCount = 20;
  public Search(
    Query: string,
    lastPage: Pagination
  ): Observable<PaginationResult<Gif>> {
    var requiredOffset = (lastPage.currentPage+1) * this.SearchPageCount;
    let params = new HttpParams()
      .set('api_key', environment.giphyApiKey)
      .set('q', Query)
      .set('limit', this.SearchPageCount)
      .set('offset', requiredOffset)
      .set('lang', 'pt');

    return this.http
      .get<any>(environment.giphyApiUrl + '/gifs/search', { params: params })
      .pipe(
        take(1),
        map((res: any) => {
          var result = {
            results: this.MapToGif(res),
            totalCount: res.pagination.total_count,
            pageSize: res.pagination.count,
          } as PaginationResult<Gif>;
          var offSet = res.pagination.offset;
          result.lastPage = result.totalCount / res.pagination.count;
          result.currentPage = offSet / result.pageSize;
          return result;
        })
      );
  }
}
