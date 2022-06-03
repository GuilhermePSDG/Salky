import { RouteFunction } from "./RouteFunction";

export interface CadastredRoute {
  routePath: string;
  method: string;
  Functions: RouteFunction[];
}
