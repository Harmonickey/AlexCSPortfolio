;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
;;; Boggle Tests
;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

(defpackage boggle-tests
  (:use :common-lisp boggle lisp-unit))

;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;

(in-package boggle-tests)

(load-words 
 (merge-pathnames "crosswd.txt"
                  (or *compile-file-truename* *load-truename*)))

(define-test boggle-2x2
  (assert-false (solve-boggle "a"))
  
  (assert-equal '("seas" "ass" "ess" "sae" "sea")
                (solve-boggle "ases"))
  
  (assert-equal '("bailie" "pulque" "abide" "aphid" "dulia" "litai" "poilu" 
                  "tabid" "uplit" "aide" "alit" "bail" "bait"
                  "bide" "diel" "dita" "edit" "eide" "hide" "hila" "hili" 
                  "hued" "hula" "ilea" "ilia" "opal" "plie" "puli"
                  "tael" "tail" "tide" "aid" "ail" "ait" "alp" "bat" "bid" 
                  "bit" "dib" "die" "dit" "due" "dup" "eat" "edh"
                  "hid" "hie" "hit" "hue" "hup" "lap" "lea" "lei" "lib" "lid" 
                  "lie" "lit" "lop" "oil" "pal" "phi" "poi"
                  "pol" "pud" "pul" "tab" "tae" "tie" "til" "upo")
                (solve-boggle "edbtuhiaplieaoql"))
  )