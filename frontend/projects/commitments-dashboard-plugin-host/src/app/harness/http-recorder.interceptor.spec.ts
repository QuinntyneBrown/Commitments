import { readFileSync } from 'fs';
import { join } from 'path';

describe('HttpRecorderInterceptor (source)', () => {
  const ts = readFileSync(join(__dirname, 'http-recorder.interceptor.ts'), 'utf8');

  it('is a functional interceptor (HttpInterceptorFn)', () => {
    expect(ts).toMatch(/HttpInterceptorFn/);
  });

  it('injects WindowBridgeService', () => {
    expect(ts).toMatch(/WindowBridgeService/);
    expect(ts).toMatch(/inject\(WindowBridgeService\)/);
  });

  it('calls bridge.recordHttp with method and url', () => {
    expect(ts).toMatch(/bridge\.recordHttp/);
    expect(ts).toMatch(/req\.method/);
    expect(ts).toMatch(/req\.urlWithParams/);
  });

  it('passes request through to next handler', () => {
    expect(ts).toMatch(/return next\(req\)/);
  });
});

describe('app.config (source)', () => {
  const ts = readFileSync(join(__dirname, '../app.config.ts'), 'utf8');

  it('wires the interceptor via withInterceptors', () => {
    expect(ts).toMatch(/withInterceptors/);
    expect(ts).toMatch(/httpRecorderInterceptor/);
  });
});
