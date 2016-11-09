import { Injectable, EventEmitter } from "@angular/core";
import { UserService } from "./user.service";

declare var $: any;

@Injectable()
export class HubService {
    private _connectionPromise;
    private _connection;
    private _callbacks: Array<Function> = [];

    constructor() {
        //this._connection = $.hubConnection("http://localhost:53723/signalr/");
        this._connection = $.hubConnection("http://flexylexy.web/signalr/");
    }

    public onConnect(callback: Function) {
        if (this._connectionPromise) {
            this._connectionPromise.done(callback);
        } else {
            this._callbacks.push(callback);
        }
    }

    public connect() {
        //var proxy = this._connection.createHubProxy("gamesHub");
        //proxy.on("ChallengeDeclined", () => {});
        this._connectionPromise = this._connection.start();


        this._connectionPromise.done(() => console.log("CONNECTED")).fail(() => console.log("NOT CONNECTED"));

        this._callbacks.forEach(callback => this._connectionPromise.done(callback));
    }

    public createProxy(proxyName: string) {
        return this._connection.createHubProxy(proxyName);
    }
}