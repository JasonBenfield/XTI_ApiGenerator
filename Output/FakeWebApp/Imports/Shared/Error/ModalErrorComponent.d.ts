import { ModalErrorComponentViewModel } from './ModalErrorComponentViewModel';
import { Command } from '../Command';
import { ErrorModel } from '../ErrorModel';
export declare class ModalErrorComponent {
    private readonly vm;
    constructor(vm: ModalErrorComponentViewModel);
    private onClosed;
    show(errors: ErrorModel[], caption?: string): void;
    readonly okCommand: Command;
    private ok;
}
