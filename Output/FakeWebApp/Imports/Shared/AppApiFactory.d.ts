import { apiConstructor, AppApi } from "./AppApi";
import { ModalErrorComponent } from "./Error/ModalErrorComponent";
export declare class AppApiFactory {
    private readonly _defaultApiType;
    private readonly modalError;
    constructor(_defaultApiType: apiConstructor<AppApi>, modalError: ModalErrorComponent);
    api<TApi extends AppApi>(apiCtor: apiConstructor<TApi>): TApi;
}
