/**
 * @id csharp/fsinfocat/local-model-interfaces
 * @name Local Model Interfaces
 * @description Finds interface inheritances for interfaces in FsInfoCat.Local.Model
 */

import csharp

from Interface interface, Interface baseInterface
where
    interface.getNamespace().getFullName() = "FsInfoCat.Local.Model" and
    (baseInterface.getNamespace().getFullName() = "FsInfoCat.Local.Model" or baseInterface.getNamespace().getFullName() = "FsInfoCat.Model") and
    interface.getABaseInterface() = baseInterface
select interface.toStringWithTypes(), baseInterface.getNamespace().getFullName() + "." + baseInterface.toStringWithTypes()

