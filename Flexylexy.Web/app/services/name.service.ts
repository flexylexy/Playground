import { Injectable, EventEmitter } from "@angular/core";
import { Observable } from "rxjs/Observable";
import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { asObservable} from "../helpers/asObservable";

declare var $: any;

@Injectable()
export class NameService {
    private _name: string;
    private _nameSubject: BehaviorSubject<string> = new BehaviorSubject("");
    public nameObservable: Observable<string> = asObservable(this._nameSubject);
    
    set name(value) {
        if (this._name) return;

        this._name = value;
        this._nameSubject.next(this._name);
    }
}