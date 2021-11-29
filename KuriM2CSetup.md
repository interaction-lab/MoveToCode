# Kuri Controllers

## TODO:
- [ ] move all old AI into respective decision scripts
- [ ] clean up any action scripts
- [ ] do utility AI

## File Descriptions
- `KuriManager`: 
  - Decides on which controller to use for actions (i.e., physical vs virtual)
  - Controls Kuri tunable params (curiosity/movement)
  - Controls the tick rate (just calls from update)
  - Keeps track of any Kuri state (e.g., `LastActionStartTime`)
- `HumanStateManager`: controls and keeps track of any human related state (e.g., curiosity and movement)
- `KuriController`: base class for all action APIS
  - `KuriPhysicalController`: action API for all physical kuri actions
  - `KuriVirtualController`: action API for all virtual kuri actions
- `KuriAI`: base class for all decision mappings
  - `KuriRuleBasedAI`: AI from original study, see first [KC paper](https://tgroechel.github.io/publications/kc.pdf) from ISER for mapping
  - `KuriUtilityAI`: AI for upcoming in school study

## Utility AI
- all values should be normalized 0-1
- variables we care about
  - movement
  - curiosity
  -  time since last action
  -  rolling time window threshold
- variable lists
  - novelty of each
- action list
  - variable actions
    - virtual ISA
    - hint/scaffold dialogue
    - PPA
    - move
- hardcoded actions
  - give exercise -> when exercise starts
  - congrats dialogue  -> when exercise completed

### Setup
```C#
// scores -> actions
// actions -> scores -> actions
// base scores combined -> composite scores -> actions
```

## [Mac Omnisharp Fix Versions](https://forum.unity.com/threads/psa-if-your-vs-code-c-extension-stopped-working.841255/page-3)