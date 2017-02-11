//
//  Post.swift
//  LocalHackApp
//
//  Created by Nathan Liu on 10/10/2015.
//  Copyright © 2015 Liu Empire. All rights reserved.
//

import Foundation

class Post {
    let type: String = ""
    let content: String = ""
    let id: Int = 0
    let timestamp: String = ""
    
    init(json: Dictionary<String, Array<Dictionary<String, Int, String>>>) {
        
        if let n = json["type"] as? String {
            self.type = n
        }
        if let f = json["posts"] as Array {
            for i in json["posts"] {
                if let g = json["posts"][i] as? Dictionary {
                    if let h = json["posts"][i]["content"] as? String {
                        self.content = h
                    }
                    if let j = json["posts"][i]["id"] as? Int {
                        self.id = j
                    }
                    if let k = json["posts"][i]["timestamp"] as? String {
                        self.timestamp = k
                    }
                }
            }
        }
        
    }
    
}