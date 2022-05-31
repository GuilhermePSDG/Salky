import { RouteFunction } from "src/app/Models/Internal/RouteFunction";

export interface CadastredRoute {
  routePath: string;
  method: string;
  Functions: RouteFunction[];
}
