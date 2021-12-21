import { ButtonCommandItem } from '../Command/ButtonCommandItem';
import { HorizontalRule } from '../Html/HorizontalRule';
import { HtmlComponent } from '../Html/HtmlComponent';
import { ModalComponentViewModel } from '../Modal/ModalComponentViewModel';
import { ModalErrorGroupComponentView } from './ModalErrorGroupComponentView';
import { ModalErrorListItem } from './ModalErrorListItem';
export declare class ModalErrorComponentView extends HtmlComponent {
    private readonly modal;
    private readonly title;
    private readonly _errorSelected;
    readonly errorSelected: import("../Events").DefaultEventHandler<ModalErrorListItem>;
    readonly closed: IEventHandler<any>;
    readonly okButton: ButtonCommandItem;
    readonly hr: HorizontalRule;
    private readonly errorGroups;
    constructor(vm?: ModalComponentViewModel);
    errorGroup(): ModalErrorGroupComponentView;
    clearErrorGroups(): void;
    setTitle(title: string): void;
    showModal(): void;
    hideModal(): void;
}
