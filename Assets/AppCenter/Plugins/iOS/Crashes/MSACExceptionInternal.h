#import <Foundation/Foundation.h>
#import "MSACException.h"

@interface MSACExceptionInternal : MSACException

@property(nonatomic) NSArray<MSACExceptionInternal *> *innerExceptions;
@property(nonatomic, copy) NSString *wrapperSdkName;

@end
