import { RouteFunction } from "src/app/Models/RouteFunction";

export interface CadastredRoute {
  routePath: string;
  method: string;
  Functions: RouteFunction[];
}
