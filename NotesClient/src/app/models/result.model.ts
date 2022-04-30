export interface Result<T> {
    errorMessage: string;
    isSuccess: boolean;
    data: T;
}