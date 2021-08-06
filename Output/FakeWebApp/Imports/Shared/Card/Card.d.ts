import { Block } from "../Html/Block";
import { BlockViewModel } from "../Html/BlockViewModel";
import { CardAlert } from "./CardAlert";
import { CardBody } from "./CardBody";
import { CardButtonListGroup } from "./CardButtonListGroup";
import { CardHeader } from "./CardHeader";
import { CardLinkListGroup } from "./CardLinkListGroup";
import { CardListGroup } from "./CardListGroup";
import { CardTitleHeader } from "./CardTitleHeader";
export declare class Card extends Block {
    constructor(vm?: BlockViewModel);
    addCardTitleHeader(title: string): CardTitleHeader;
    addCardHeader(): CardHeader;
    addCardAlert(): CardAlert;
    addCardBody(): CardBody;
    addButtonListGroup(): CardButtonListGroup;
    addLinkListGroup(): CardLinkListGroup;
    addListGroup(): CardListGroup;
}
