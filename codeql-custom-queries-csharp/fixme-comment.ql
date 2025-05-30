/**
 * @name FIXME comments
 * @kind problem
 * @problem.severity warning
 * @id csharp/fsinfocat/fixme-comment
 * @description Finds comments containing the word "FIXME".
 */

import csharp

from CommentLine c
where c.getText().regexpMatch("(?si).*\\bFIXME\\b.*")
select c, c.getText()
