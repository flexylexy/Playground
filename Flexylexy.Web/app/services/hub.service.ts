import { Injectable, EventEmitter } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { Subject } from "rxjs/Subject";
import { asObservable} from "../helpers/asObservable";
import { NameService } from "./name.service";
 
declare var $: any;

@Injectable()
export class HubService {
    private proxy;
    private connection;

    private opponentPlaySubject: Subject<any> = new Subject();
    private challengedSubject: Subject<any> = new Subject();
    private challengeAcceptedSubject: Subject<any> = new Subject();
    private challengeDeclinedSubject: Subject<any> = new Subject();
    private playersUpdatedSubject: Subject<any> = new Subject();

    public opponentPlay: Observable<any> = asObservable(this.opponentPlaySubject);
    public challenged: Observable<any> = asObservable(this.challengedSubject);
    public challengeAccepted: Observable<any> = asObservable(this.challengeAcceptedSubject);
    public challengeDeclined: Observable<any> = asObservable(this.challengeDeclinedSubject);
    public playersUpdated: Observable<any> = asObservable(this.playersUpdatedSubject);

    constructor(private _nameService: NameService) {
        this.connection = $.hubConnection("http://localhost:53723/signalr/");
        this.proxy = this.connection.createHubProxy("gamesHub");


        this.registerOnServerEvents();
        this.connect();
    }

    private connect() {
        this.connection.start().done(() => {
            this._nameService.nameObservable.subscribe(name => this.proxy.invoke("AddClient", name));
        });
    }

    public getConnectionId() {
        return this.connection.id;
    }

    public play(position: number, opponentConnectionId: string) {
        this.proxy.invoke("Play", position, opponentConnectionId);
    }

    // TEST
    public clearPlayers() {
        this.proxy.invoke("ClearPlayers");
    }
    // TEST

    public challenge(opponentName: string, opponentConnectionId: string) {
        this.proxy.invoke("Challenge", opponentName, opponentConnectionId);
    }

    public acceptChallenge(opponentConnectionId: string) {
        this.proxy.invoke("AcceptChallenge", opponentConnectionId);
    }

    public declineChallenge(opponentConnectionId: string) {
        this.proxy.invoke("DeclineChallenge", opponentConnectionId);
    }

    public getPlayers() {
        return $.ajax({
            url: "Home/GetPlayers"
        });
    }

    private registerOnServerEvents() {
        this.proxy.on("Play", (opponentConnectionId, position) => {
            this.opponentPlaySubject.next({ opponentConnectionId: opponentConnectionId, position: position });
        });

        this.proxy.on("ChallengeAccepted", (opponentConnectionId) => {
            this.challengeAcceptedSubject.next(opponentConnectionId);
        });

        this.proxy.on("ChallengeDeclined", () => {
            this.challengeDeclinedSubject.next();
        });

        this.proxy.on("Challenged", (opponentName, opponentConnectionId) => {
            this.challengedSubject.next({ opponentName: opponentName, opponentConnectionId: opponentConnectionId });
        });

        this.proxy.on("PlayersUpdated", (players) => {
            this.playersUpdatedSubject.next({ players: players, connectionId: this.connection.id });
        });
    }
}