import { Injectable, EventEmitter } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Subject } from "rxjs/Subject";
import { asObservable} from "../helpers/asObservable";
import { HubService } from "./hub.service";
import { UserService } from "./user.service";
import {Player } from "../types/player";
declare var $: any;

@Injectable()
export class ChatService {
    private proxy;

    private receivedChatSubject: Subject<any> = new Subject();
    public receivedChat: Observable<any> = asObservable(this.receivedChatSubject);

    constructor(
        private _userService: UserService,
        private _hubService: HubService
    ) {
        this.proxy = this._hubService.createProxy("chatHub");
        this.registerOnServerEvents();
    }

    public sendChat(content: string, player?: Player) {
        this.proxy.invoke("SendChat", content, player);
    }

    private registerOnServerEvents() {
        this.proxy.on("ChatReceived", (message: string, sender: Player) => {
            this.receivedChatSubject.next({ message: message, sender: sender });
        });
    }
}