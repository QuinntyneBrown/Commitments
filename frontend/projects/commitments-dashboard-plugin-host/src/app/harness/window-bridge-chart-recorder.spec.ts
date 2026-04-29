import { readFileSync } from 'fs';
import { join } from 'path';

describe('WindowBridgeChartRecorder (source)', () => {
  const ts = readFileSync(join(__dirname, 'window-bridge-chart-recorder.ts'), 'utf8');

  it('implements ChartRecorder', () => {
    expect(ts).toMatch(/implements ChartRecorder/);
  });

  it('is provided in root', () => {
    expect(ts).toMatch(/providedIn:\s*['"]root['"]/);
  });

  it('calls bridge.recordChart in onAttach with kind=attach', () => {
    expect(ts).toMatch(/bridge\.recordChart/);
    expect(ts).toMatch(/kind:\s*['"]attach['"]/);
  });

  it('calls bridge.recordChart in onUpdateDataset with kind=updateDataset', () => {
    expect(ts).toMatch(/kind:\s*['"]updateDataset['"]/);
  });

  it('calls bridge.recordChart in onDestroy with kind=destroy', () => {
    expect(ts).toMatch(/kind:\s*['"]destroy['"]/);
  });
});

describe('app.config CHART_RECORDER provider (source)', () => {
  const ts = readFileSync(join(__dirname, '../app.config.ts'), 'utf8');

  it('provides CHART_RECORDER using WindowBridgeChartRecorder', () => {
    expect(ts).toMatch(/CHART_RECORDER/);
    expect(ts).toMatch(/WindowBridgeChartRecorder/);
  });
});
