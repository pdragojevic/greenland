import { Task } from "./Task";

export class Column{
    constructor(public name: string, public tasks: Task[]) {}
}