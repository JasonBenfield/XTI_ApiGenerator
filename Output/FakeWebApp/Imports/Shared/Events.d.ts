export declare class EventCollection {
    constructor();
    private readonly _identifier;
    private readonly _events;
    register<TArgs>(evt: IEventHandler<TArgs>, callback: EventCallback<TArgs>, isEnabled?: () => boolean): this;
    dispose(): void;
}
export declare class DefaultEvent<TArgs> implements IEvent<TArgs> {
    private readonly source;
    private static defaultIsEnabled;
    constructor(source: any);
    private readonly _callbacks;
    register(callback: EventCallback<TArgs>, identifier?: any, isEnabled?: () => boolean): this;
    invoke(args: TArgs): void;
    unregister(identifier: any): void;
    dispose(): void;
}
export declare class DefaultEventHandler<TArgs> implements IEventHandler<TArgs> {
    private readonly source;
    constructor(source: IEvent<TArgs>);
    register(callback: EventCallback<TArgs>, identifier?: any, isEnabled?: () => boolean): IEvent<TArgs>;
    unregister(identifier: any): void;
}
export declare class SimpleEvent extends DefaultEvent<any> {
    constructor(source: any);
    invoke(): void;
}
