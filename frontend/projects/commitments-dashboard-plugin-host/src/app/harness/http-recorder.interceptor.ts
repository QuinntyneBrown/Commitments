import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { WindowBridgeService } from './window-bridge.service';

export const httpRecorderInterceptor: HttpInterceptorFn = (req, next) => {
  const bridge = inject(WindowBridgeService);
  const params: Record<string, string> = {};
  for (const k of req.params.keys()) {
    params[k] = req.params.get(k) ?? '';
  }
  bridge.recordHttp({ method: req.method, url: req.urlWithParams, params });
  return next(req);
};
