import { PluginHarnessSnapshot } from './window-bridge.service';

declare global {
  interface Window {
    __pluginHarness: PluginHarnessSnapshot;
  }
}
