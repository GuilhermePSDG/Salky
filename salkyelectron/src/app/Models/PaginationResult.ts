import { Pagination } from "./Pagination";

export interface PaginationResult<T> extends Pagination {
  results : T[];
}
