/**
 * @id csharp/fsinfocat/base-model-interfaces
 * @name Base Model Interfaces
 * @description Finds interface inheritances for interfaces in FsInfoCat.Model
 */

import csharp

from Interface interface, Interface baseInterface
where
    interface.getNamespace().getFullName() = "FsInfoCat.Model" and
    baseInterface.getNamespace().getFullName() = "FsInfoCat.Model" and
    baseInterface = interface.getABaseInterface()
select interface.toStringWithTypes(), baseInterface.toStringWithTypes()
